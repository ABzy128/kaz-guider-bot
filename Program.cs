using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

Kazakhstan kazakhstan = new();
kazakhstan.fetch();
if(kazakhstan.API == null) return;
var botClient = new TelegramBotClient(kazakhstan.API);

using var cts = new CancellationTokenSource();

var recieverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>(),
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: recieverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Hello I am @{me.Username}");
while(true)
{
    var input = Console.ReadLine();
    if(input == "exit") break;
}

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken){
    if(update.Message is not { } message)
        return;
    if(message.Text is not { } messageText)
        return;
    Console.WriteLine($"({message.Chat.Id}) {message.Chat.FirstName} {message.Chat.LastName} sent {message.Text}");

    if(messageText.StartsWith("/start", StringComparison.OrdinalIgnoreCase)){
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        { 
            new KeyboardButton("Start"),
        });
        replyKeyboardMarkup.ResizeKeyboard = true;
        Message sendmessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat,
            text: kazakhstan.WelcomeMessage, 
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken
        );
    }

    else if(messageText == "Start" || messageText == "Back to main menu"){
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]{
                new KeyboardButton[] {"General Info", "History"},
                new KeyboardButton[] {"Geography", "Culture"},
                new KeyboardButton[] {"Economy", "Regions"},
                new KeyboardButton[] {"Help"},
        });
        replyKeyboardMarkup.ResizeKeyboard = true;
        Message sendmessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat,
            text: kazakhstan.WelcomeMessage, 
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken
        );
    }

    else if(messageText == "General Info"){
        Message sendmessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat,
            text: kazakhstan.GeneralInfo, 
            cancellationToken: cancellationToken
        );
    }

    else if(messageText == "History"){
        Message sendmessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat,
            text: kazakhstan.History, 
            cancellationToken: cancellationToken
        );
    }

    else if(messageText == "Geography"){
        Message sendmessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat,
            text: kazakhstan.Geography, 
            cancellationToken: cancellationToken
        );
    }

    else if(messageText == "Culture"){
        Message sendmessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat,
            text: kazakhstan.Culture, 
            cancellationToken: cancellationToken
        );
    }

    else if(messageText == "Economy"){
        Message sendmessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat,
            text: kazakhstan.Economy, 
            cancellationToken: cancellationToken
        );
    }
    else if(messageText == "Help"){
        Message sendmessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat,
            text: kazakhstan.HelpMessage, 
            cancellationToken: cancellationToken
        );
    }
    else if(messageText == "Regions" || messageText == "Back to regions menu"){
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]{
            kazakhstan.Regions.Select(region => new KeyboardButton(region.Name)).ToArray(),
            new KeyboardButton[] {"Back to main menu"}, 
        });
        replyKeyboardMarkup.ResizeKeyboard = true;
        Message sendmessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat,
            text: "Choose a region",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken
        );
    }
    foreach(var region in kazakhstan.Regions){
        if(region.Name == messageText){
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]{
                new KeyboardButton[] {$"General Info of {region.Name}"},
                new KeyboardButton[] {$"Landmarks of {region.Name}"},
                new KeyboardButton[] {$"Back to regions menu"},
            });
            replyKeyboardMarkup.ResizeKeyboard = true;
            Message sendmessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat,
                text: "You chose " + region.Name,
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }
    }
    foreach(var region in kazakhstan.Regions){
        if(messageText == $"General Info of {region.Name}"){
            Message sendmessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat,
                text: region.GeneralInfo, 
                cancellationToken: cancellationToken
            );
        }
        else if(messageText == $"Landmarks of {region.Name}"){
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]{
                region.Landmarks.Select(landmark => new KeyboardButton(landmark.Name)).ToArray(),
                new KeyboardButton[] {"Back to regions menu"}, 
            });
            replyKeyboardMarkup.ResizeKeyboard = true;
            Message sendmessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat,
                text: "Choose a landmark", 
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }
    }
    foreach(var region in kazakhstan.Regions){
        foreach(var landmark in region.Landmarks){
            if(messageText == landmark.Name){
                Message sendmessage1 = await botClient.SendPhotoAsync(
                    chatId: message.Chat,
                    photo: landmark.Image,
                    caption: landmark.Name,
                    cancellationToken: cancellationToken
                );
                Message sendmessage2 = await botClient.SendTextMessageAsync(
                    chatId: message.Chat,
                    text: landmark.Description, 
                    cancellationToken: cancellationToken
                );
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]{
                    new KeyboardButton[] {"Back to regions menu"}, 
                });
                replyKeyboardMarkup.ResizeKeyboard = true;
                Message sendmessage3 = await botClient.SendLocationAsync(
                    chatId: message.Chat,
                    latitude: Convert.ToDouble(landmark.Location.Substring(0, 7)),
                    longitude: Convert.ToDouble(landmark.Location.Substring(9, 7)),
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken
                );
            }
        }
    }
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken){
    var error = exception switch{
        ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine("Error occured:\n" + error);
    return Task.CompletedTask;   
}