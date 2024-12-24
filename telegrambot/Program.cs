using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using GuiStracini.HolidayAPI;
using System.Net.Http.Headers;
using Telegram.Bot.Types.ReplyMarkups;
using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("СЮДА НАДО КЛЮЧ ТЕЛЕГРАМБОТАПИ");
InlineKeyboardMarkup keyboard = new(InlineKeyboardButton.WithCallbackData("Праздники сегодня"));
var client = HttpClientFactory.Create();
client.BaseAddress = new Uri("https://holidayapi.com/");
client.DefaultRequestHeaders.ExpectContinue = false;
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
var myHolidayKey = "СЮДА НАДО КЛЮЧ ХОЛИДЕЙАПИ";
var HolidayClient = new HolidayApiClient(myHolidayKey, client);
Dictionary<string, string> countries = new Dictionary<string, string>()
{
    { "RU", "Россия"},
    { "AD", "Андорра"},
    { "AE", "Объединенные Арабские Эмираты"},
    { "AF", "Афганистан"},
    { "AG", "Антигуа и Барбуда"},
    { "AI", "Ангилья"},
    { "AL", "Албания"},
    { "AM", "Армения"},
    { "AO", "Анголла"},
    { "AQ", "Антарктида"},
    { "AR", "Аргентина"},
    { "AS", "Американское Самоа"},
    { "AT", "Австрия"},
    { "AU", "Австралия"},
    { "AW", "Аруба"},
    { "AX", "Аландские острова"},
    { "AZ", "Азербайджан"},
    { "BA", "Босния и Герцеговина"},
    { "BB", "Барбадос"},
    { "BD", "Бангладеш"},
    { "BE", "Бельгия"},
    { "BF", "Буркина-Фасо"},
    { "BG", "Болгария"},
    { "BH", "Бахрейн"},
    { "BI", "Бурунди"},
    { "BJ", "Бенин"},
    { "BL", "Сан-Бартелеми"},
    { "BM", "Бермуда"},
    { "BN", "Бруней-Дарусаллам"},
    { "BO", "Боливия"},
    { "BQ", "Карибские Нидерланды"},
    { "BR", "Бразилия"},
    { "BS", "Содружество Багамских островов"},
    { "BT", "Бохтан"},
    { "BV", "Остров Буве"},
    { "BW", "Ботсвана"},
    { "BY", "Беларусь"},
    { "BZ", "Белиз"},
    { "CA", "Канада"},
    { "CC", "Кокосовые острова"},
    { "CD", "Демократическая Республика Конго"},
    { "CF", "Центральноафриканская Республика"},
    { "CG", "Республика Конго"},
    { "CH", "Цвейцария"},
    { "CI", "Кот-д’Ивуар"},
    { "CK", "Острова Кука"},
    { "CL", "Чили"},
    { "CM", "Камерун"},
    { "CN", "Китай"},
    { "CO", "Колумбия"},
    { "CR", "Коста-Рика"},
    { "CU", "Республика Куба"},
    { "CV", "Кабо-Верде"},
    { "CW", "Кюрасао"},
    { "CX", "Остров Рождества"},
    { "CY", "Республика Кипр"},
    { "CZ", "Чехия"},
    { "DE", "Германия"},
    { "DJ", "Джибути"},
    { "DK", "Дания"},
    { "DM", "Доминика"},
    { "DO", "Доминиканская Республика"},
    { "DZ", "Алжир"},
    { "EC", "Эквадор"},
    { "EE", "Эстония"},
    { "EG", "Египет"},
    { "EH", "Сахарская Арабская Демократическая Республика"},
    { "ER", "Эритрея"},
    { "ES", "Испания"},
    { "ET", "Эфиопия"},
    { "FI", "Финляндия"},
    { "FJ", "Фиджи"},
    { "FK", "Фолклендские острова"},
    { "FM", "Микронезия"},
    { "FO", "Фарерския острова"},
    { "FR", "Франция"},
    { "GA", "Габон"},
    { "GB", "Великобритания"},
    { "GD", "Гренада"},
    { "GE", "Грузия"},
    { "GF", "Гвиана"},
    { "GG", "Гернси"},
    { "GH", "Гана"},
    { "GI", "Гибралтар"},
    { "GL", "Гренландия"},
    { "GM", "Гамбия"},
    { "GN", "Гвинея"},
    { "GP", "Гваделупа"},
    { "GQ", "Экваториальная Гвинея"},
    { "GR", "Греция"},
    { "GS", "Южная Георгия"},
    { "GT", "Гватемала"},
    { "GU", "Гуам"},
    { "GW", "Гвинея-Бисау"},
    { "GY", "Гайана"},
    { "HK", "Гонконг"},
    { "HM", "Остров Херд и острова Макдональд"},
    { "HN", "Гондурас"},
    { "HR", "Хорватия"},
    { "HT", "Гаити"},
    { "HU", "Венгрия"},
    { "ID", "Индонезия"},
    { "IE", "Ирландия"},
    { "IL", "Израиль"},
    { "IM", "Остров Мэн"},
    { "IN", "Индия"},
    { "IO", "Британская Территория в Индийском Океане"},
    { "IQ", "Ирак"},
    { "IR", "Иран"},
    { "IS", "Исландия"},
    { "IT", "Италия"},
    { "JE", "Джерси"},
    { "JM", "Ямайка"},
    { "JO", "Иордания"},
    { "JP", "Япония"},
    { "KE", "Кения"},
    { "KG", "Кыргызстан"},
    { "KH", "Камбоджа"},
    { "KI", "Кирибати"},
    { "KM", "Коморы"},
    { "KN", "Сент-Китс и Невис"},
    { "KP", "Северная Корея"},
    { "KR", "Южная Корея"},
    { "KW", "Кувейт"},
    { "KY", "Острова Кайман"},
    { "KZ", "Казахстан"},
    { "LA", "Лаос"},
    { "LB", "Ливан"},
    { "LC", "Сент-Люсия"},
    { "LI", "Лихтенштейн"},
    { "LK", "Шри-Ланка"},
    { "LR", "Либерия"},
    { "LS", "Лесото"},
    { "LT", "Литва"},
    { "LU", "Люксембург"},
    { "LV", "Латвия"},
    { "LY", "Ливия"},
    { "MA", "Марокко"},
    { "MC", "Монако"},
    { "MD", "Молдавия"},
    { "ME", "Черногория"},
    { "MF", "Сен-Мартен"},
    { "MG", "Мадагаскар"},
    { "MH", "Маршалловы Острова"},
    { "MK", "Северная Македония"},
    { "ML", "Мали"},
    { "MM", "Мьянма"},
    { "MN", "Монголия"},
    { "MO", "Макао"},
    { "MP", "Северные Марианские Острова"},
    { "MQ", "Мартиника"},
    { "MR", "Мавритания"},
    { "MS", "Монтсеррат"},
    { "MT", "Мальта"},
    { "MU", "Маврикий"},
    { "MV", "Мальдивы"},
    { "MX", "Мексика"},
    { "MY", "Малазия"},
    { "MZ", "Мозамбик"},
    { "NA", "Намибия"},
    { "NC", "Новая Каледония"},
    { "NE", "Нигер"},
    { "NF", "Остров Норфолк"},
    { "NG", "Нигерия"},
    { "NI", "Никарагуа"},
    { "NL", "Нидерланды"},
    { "NO", "Норвегия"},
    { "NP", "Непал"},
    { "NR", "Науру"},
    { "NU", "Ниуэ"},
    { "NZ", "Новая Зеландия"},
    { "OM", "Оман"},
    { "PA", "Панама"},
    { "PE", "Перу"},
    { "PF", "Французская Полинезия"},
    { "PG", "Папуа — Новая Гвинея"},
    { "PH", "Филиппины"},
    { "PK", "Пакистан"},
    { "PL", "Польша"},
    { "PM", "Сен-Пьер и Микелон"},
    { "PR", "Пуэрто-Рико"},
    { "PS", "Палестина"},
    { "PT", "Португалия"},
    { "PW", "Палау"},
    { "PY", "Парагвай"},
    { "QA", "Катар"},
    { "RE", "Реюньон"},
    { "RO", "Румыния"},
    { "RS", "Сербия"},
    { "RW", "Руанда"},
    { "SA", "Саудовская Аравия"},
    { "SB", "Соломоновы Острова"},
    { "SC", "Сейшельские Острова"},
    { "SD", "Судан"},
    { "SE", "Швеция"},
    { "SG", "Сингапур"},
    { "SH", "Острова Святой Елены, Вознесения и Тристан-да-Кунья"},
    { "SI", "Словения"},
    { "SJ", "Шпицберген и Ян-Майен"},
    { "SK", "Словакия"},
    { "SL", "Сьерра-Леоне"},
    { "SM", "Сан-Марино"},
    { "SN", "Сенегал"},
    { "SO", "Сомали"},
    { "SR", "Суринам"},
    { "SS", "Южный Судан"},
    { "ST", "Сан-Томе и Принсипи"},
    { "SV", "Сальвадор"},
    { "SX", "Синт-Мартен"},
    { "SY", "Сирия"},
    { "SZ", "Эсватини"},
    { "TC", "Теркс и Кайкос"},
    { "TD", "Чад"},
    { "TF", "Французские Южные и Антарктические территории"},
    { "TG", "Того"},
    { "TH", "Тайланд"},
    { "TJ", "Таджикистан"},
    { "TK", "Токелау"},
    { "TL", "Восточный Тимор"},
    { "TM", "Туркменистан"},
    { "TN", "Тунис"},
    { "TO", "Тонга"},
    { "TR", "Турция"},
    { "TT", "Тринидад и Тобаго"},
    { "TV", "Тувалу"},
    { "TW", "Тайвань"},
    { "TZ", "Танзания"},
    { "UA", "Украина"},
    { "UG", "Уганда"},
    { "UM", "Внешние малые острова США"},
    { "UN", "ООН"},
    { "US", "США"},
    { "UY", "Уругвай"},
    { "UZ", "Узбекистан"},
    { "VA", "Ватикан"},
    { "VC", "Сент-Винсент и Гренадины"},
    { "VE", "Венесуэла"},
    { "VG", "Британские Виргинские Острова"},
    { "VI", "Американские Виргинские Острова"},
    { "VN", "Вьетнам"},
    { "VU", "Вануату"},
    { "WF", "Уоллис и Футуна"},
    { "WS", "Самоа"},
    { "XS", "Республика Косово"},
    { "YE", "Йемен"},
    { "YT", "Майотта"},
    { "ZA", "ЮАР"},
    { "ZM", "Замбия"},
    { "ZW", "Зимбабве"}
};
Dictionary<DateTime, string> cache = new Dictionary<DateTime, string>();
var me = await bot.GetMeAsync();
var receiverOptions = new ReceiverOptions();
bot.StartReceiving(HandleUpdate, HandleError, cancellationToken: cts.Token);
Console.WriteLine($"{me.Id}   {me.FirstName}");
while (Console.ReadLine() != "Stop")
{}
cts.Cancel();

async Task HandleUpdate(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
{
    try
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await HandleMessage(update.Message!);
                break;
            case UpdateType.CallbackQuery:
                await HandleButton(update.CallbackQuery!);
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("ДУДОСЯТ\n" + ex);
    }
}
async Task HandleError(ITelegramBotClient _, Exception exception, CancellationToken cancellationToken)
{
    await Console.Error.WriteLineAsync(exception.Message);
}

async Task HandleButton(CallbackQuery query)
{
    if (query.Data == "Праздники сегодня")
    {
        Message message = new Message();
        message.Text = "Today";
        message.From = query.From;
        HandleMessage(message);
    }
    await bot.AnswerCallbackQueryAsync(query.Id);
}

async Task HandleMessage(Message message)
{
    Console.WriteLine($"Message from {message.From.Id}: '{message.Text}' ");
    switch (message.Text)
    {
        case null:
            {
                return; 
            }
        case "Today":
            {
                DateTime Today = System.DateTime.Today;
                if (cache.TryGetValue(Today, out var cachedResponse))
                {
                    await bot.SendTextMessageAsync(message.From.Id, cachedResponse, replyMarkup: keyboard);
                    return;
                }
                await bot.SendTextMessageAsync(message.From.Id, "Собираю информацию о праздниках...");
                string response = "Праздники сегодня: \n";
                int counter = 0;
                HolidayFilter filter = new HolidayFilter("ru, ad, ae, af, ag, ai, al, am, ao, aq", 2023);
                filter.Language = "RU";
                var holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date == Today && holiday.Date == Today)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("ar, as, at, au, aw, ax, az, ba, bb, bd", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("be, bf, bg, bh, bi, bj, bl, bm, bn, bo", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("bq, br, bs, bt, bv, bw, by, bz, ca, cc", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("cd, cf, cg, ch, ci, ck, cl, cm, cn, co", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("cr, cu, cv, cw, cx, cy, cz, de, dj, dk", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("dm, do, dz, ec, ee, eg, eh, er, es, et", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("fi, fj, fk, fm, fo, fr, ga, gb, gd, ge", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("gf, gg, gh, gi, gl, gm, gn, gp, gq, gr", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("gs, gt, gu, gw, gy, hk, hm, hn, hr, ht", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("hu, id, ie, il, im, in, io, iq, ir, is", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("it, je, jm, jo, jp, ke, kg, kh, ki, km", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("kn, kp, kr, kw, ky, kz, la, lb, lc, li", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("lk, lr, ls, lt, lu, lv, ly, ma, mc, md", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("me, mf, mg, mh, mk, ml, mm, mn, mo, mp", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("mq, mr, ms, mt, mu, mv, mw, mx, mz, na", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("nc, ne, nf, ng, ni, nl, no, np, nr, nu", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("nz, om, pa, pe, pf, pg, ph, pk, pl, pm", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("pn, pr, ps, pt, pw, py, qa, re, ro, rs", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("rw, sa, sb, sc, sd, se, sg, sh, si, sj", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("sk, sl, sm, sn, so, sr, ss, st, sv, sx", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("sy, sz, tc, td, tf, tg, th, tj, tk, tl", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("tm, tn, to, tr, tt, tv, tw, tz, ua, ug", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("um, un, us, uy, uz, va, vc, ve, vg, vi", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                filter = new HolidayFilter("vn, vu, wf, ws, xk, ye, yt, za, zm, zw", 2023);
                filter.Language = "RU";
                holidays = await HolidayClient.GetHolidaysAsync(filter, CancellationToken.None);
                foreach (var holiday in holidays)
                {
                    if (holiday.Date.Day == Today.Day && holiday.Date.Month == Today.Month)
                    {
                        counter++;
                        response += $"{counter}. {countries[holiday.Country]}. {holiday.Name}\n";
                    }
                }
                if (counter == 0)
                {
                    await bot.SendTextMessageAsync(message.From.Id, "Сегодня в мире нет праздников :(", replyMarkup: keyboard);
                }
                else
                    await bot.SendTextMessageAsync(message.From.Id, response, replyMarkup: keyboard);
                cache.Add(Today, response);
                Console.WriteLine($"DONE SENDING TO {message.From.Id}");
                break;
            }

    }
}