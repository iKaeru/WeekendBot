using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Weekend.Enums;
using Weekend.Helpers;
using Weekend.Models;
using BotCommands = Weekend.Enums.BotCommands;

namespace Weekend.Workers
{
	public static class TextsWorker
	{
		private static ITelegramBotClient _botClient;

		private static User _botInfo;
		private static int _maxResponseSkip = 3;
		private static int _currentFlipAndClickResponseIndex = _maxResponseSkip;

		public static void Init(ITelegramBotClient botClient, User botInfo)
		{
			_botClient = botClient;
			_botInfo = botInfo;
		}

		public static InlineKeyboardMarkup CreateInlineButtonsForReply()
		{
			return new InlineKeyboardMarkup(new[]
			{
				new[]
				{
					InlineKeyboardButton.WithCallbackData("Нажми 1", "1"),
					InlineKeyboardButton.WithCallbackData("Нажми 2", "2"),
					InlineKeyboardButton.WithCallbackData("Нажми 3", "3"),
				}
			});
		}

		public static async Task ReplyInPersonalChat(Message message)
		{
			var messageText = message.Text.ToLower();
			if (messageText.Contains("ping") || messageText.Contains("пинг"))
			{
				await SendReply(message, $"Я живой {Emojis.GetRandomHexadecimalEmoji()}!");
			}
		}

		public static async Task ReplyInGroupChat(Message message)
		{
			if (await ProcessCommand(message))
			{
				return;
			}

			var messageText = message.Text.ToLower();

			if (IsReplyAvailable(ref _currentFlipAndClickResponseIndex))
			{
				if (messageText.Contains("щёлк") || messageText.Contains("щелк") || messageText.Contains("щелч")
				    || messageText.Contains("щёлч") || messageText.Contains("счелк") || messageText.Contains("счёлк"))
				{
					await SendReply(message, $"Флип {Emojis.GetRandomHexadecimalEmoji()}");
					return;
				}

				if (messageText.Contains("флип"))
				{
					await SendReply(message, $"Щёлк {Emojis.GetRandomHexadecimalEmoji()}");
					return;
				}
			}

			foreach (var stickerValue in (ChatSticker[]) Enum.GetValues(typeof(ChatSticker)))
			{
				if (StickersStorage.IsStickerTriggered(stickerValue, messageText) &&
				    StickersStorage.IsResponseAvailable(stickerValue))
				{
					await SendSticker(message, StickersStorage.GetStickerId(stickerValue));
					return;
				}
			}
		}

		private async static Task<bool> ProcessCommand(Message message)
		{
			foreach (var botCommand in (BotCommands[]) Enum.GetValues(typeof(BotCommands)))
			{
				if (message.Text.StartsWith($"/{botCommand.GetString()}@{_botInfo.Username}"))
				{
					string reply;
					switch (botCommand)
					{
						case BotCommands.RandomTricks:
							reply = TricksProcessor.ProcessRandomTrick(message);
							await SendReply(message, reply);
							await SendReplyWithFormatting(message, "Присылай трюк с хештегом <b><u>ответрандом</u></b>");
							break;
						case BotCommands.HardTricks:
							reply = TricksProcessor.ProcessHardTrick(message);
							await SendReply(message, reply);
							await SendReplyWithFormatting(message, "Присылай трюк с хештегом <b><u>ответхард</u></b>");
							break;
						case BotCommands.RemoveTrick:
							await SendReply(message, "Ведутся технические работы...");
							break;
						case BotCommands.ClearAllTricks:
							if (await IsSenderActiveAdmin(message))
							{
								UserOwnedTricksParser.CleanAllOwnedTricks();
								await SendReply(message, "Все трюки для всех удалены 👀");
							}
							else
							{
								await SendReply(message, "У тебя здесь нет власти!");
							}

							break;
						case BotCommands.AdminsList:
							var activeAdmins = await GetAllActiveAdmins(message);
							await SendReply(message, "Вот список всех текущих администраторов:\n" +
							                         $"{string.Join(", ", activeAdmins.Select(admin => $"@{admin.User.Username}"))}");
							break;
						default:
							Console.WriteLine("Didn't found command name");
							break;
					}

					return true;
				}
			}


			return false;
		}

		private static bool IsReplyAvailable(ref int currentIndex)
		{
			currentIndex++;

			if (currentIndex < _maxResponseSkip)
				return false;

			currentIndex = 0;
			return true;
		}

		private static async Task<ChatMember[]> GetAllActiveAdmins(Message message)
		{
			var allAdmins = await GetAllChatAdmins(message);
			return allAdmins
				.Where(admin => admin.Status == ChatMemberStatus.Creator || (admin.CanDeleteMessages??=false) && (admin.CanRestrictMembers??=false) && !admin.User.IsBot)
				.ToArray();
		}

		private static async Task<bool> IsSenderActiveAdmin(Message message)
		{
			var chatMember = await GetChatMember(message);
			var allActiveAdmins = await GetAllActiveAdmins(message);
			return allActiveAdmins.Any(admins => admins.User.Id == chatMember.User.Id);
		}

		private static async Task<ChatMember[]> GetAllChatAdmins(Message message)
		{
			return await _botClient.GetChatAdministratorsAsync(message.Chat.Id);
		}

		private static async Task<ChatMember> GetChatMember(Message message)
		{
			return await _botClient.GetChatMemberAsync(message.Chat.Id, message.From.Id);
		}

		private static async Task SendReply(Message message, string replyMsg)
		{
			await _botClient.SendTextMessageAsync(
				chatId: message.Chat,
				text: replyMsg,
				replyToMessageId: message.MessageId
			);
		}

		private static async Task SendReplyWithFormatting(Message message, string replyMsg)
		{
			await _botClient.SendTextMessageAsync(
				chatId: message.Chat,
				parseMode: ParseMode.Html,
				text: replyMsg,
				replyToMessageId: message.MessageId
			);
		}

		private static async Task SendSticker(Message message, string replyStickerId)
		{
			await _botClient.SendStickerAsync(
				chatId: message.Chat,
				sticker: replyStickerId,
				replyToMessageId: message.MessageId
			);
		}
	}
}