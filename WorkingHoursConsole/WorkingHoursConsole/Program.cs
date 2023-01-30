// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

CalculateWorkingHours();

void CalculateWorkingHours()
{
    string appSettingJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
    var appSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingJson);

    var jsonDefaultPath = appSettings.JsonFilePath;
    JsonItem jsonItem = null;
    var answer = string.Empty;
    while (answer != "n" && answer != "y")
    {
        Console.WriteLine("Wanna use the default path?(y/n)");
        answer = Console.ReadLine().ToLower();
    }
    if (answer == "n")
    {
        var newJsonPath = string.Empty;
        while (jsonItem == null)
        {
            Console.WriteLine("Write Json Path...");
            newJsonPath  = Console.ReadLine();

            jsonItem = ReadJson(newJsonPath);
        }
        var save = string.Empty;

        while (save != "n" && save != "y")
        {
            Console.WriteLine("Wanna save it as a new path?(y/n)");
            save = Console.ReadLine().ToLower();
        }

        if (save == "y")
        {
            appSettings.JsonFilePath = newJsonPath;
            var newJsonFile = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), newJsonFile);
        }
    }
    else
    {
        jsonItem = ReadJson(jsonDefaultPath);
    }
    jsonItem.Total = jsonItem.WorkingHours.Sum(x => x.TotalTime);
    jsonItem.TotalRemainingWorkingHours = appSettings.MonthlyWorkingHours > jsonItem.WorkingHours.Sum(x => x.TotalTime) ? appSettings.MonthlyWorkingHours - jsonItem.WorkingHours.Sum(x => x.TotalTime) : 0;
    jsonItem.TotalOvertime = jsonItem.WorkingHours.Sum(x => x.TotalTime) > appSettings.MonthlyWorkingHours ? jsonItem.WorkingHours.Sum(x => x.TotalTime) - appSettings.MonthlyWorkingHours : 0;
    Console.WriteLine($"Total Hour : {jsonItem.Total}");
    Console.WriteLine($"Total Remaining Working Hours : {jsonItem.TotalRemainingWorkingHours}");
    Console.WriteLine($"Total Overtime : {jsonItem.TotalOvertime}");
    var theEndOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
    var today = DateTime.Now.ToString("dd-MM-yyyy");
    var yesterday = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
    var todayWorkingHours = jsonItem.WorkingHours.Where(x => x.Date.Contains(today)).ToList();
    var yesterdayWorkingHours = jsonItem.WorkingHours.Where(x => x.Date.Contains(yesterday)).ToList();
    CalculateTheWorkingHours(theEndOfMonth, appSettings.TheGoal, appSettings.MonthlyWorkingHours, jsonItem.Total, todayWorkingHours.Sum(x => x.TotalTime), yesterdayWorkingHours.Sum(x => x.TotalTime));
    jsonItem.WorkingHours = jsonItem.WorkingHours.Select(x => 
                                                             {
                                                                 if (x.TotalTime > 0) 
                                                                 { 
                                                                     x.Status = true; 
                                                                 } 
                                                                 return x;
                                                             }); 
    File.WriteAllText(appSettings.JsonFilePath, JsonConvert.SerializeObject(jsonItem, Formatting.Indented));
    Console.WriteLine("Press enter to close...");
    Console.ReadLine();
}
void CalculateTheWorkingHours(int theEndOfMonth, int theGoal, int monthlyWorkingHours, decimal totalTime, decimal todayWorkingHours, decimal yesterdayWorkingHours)
{
    var theNumberOfRemainingDays = theEndOfMonth - DateTime.Now.AddDays(-1).Day;
    var theNumberOfOldRemainingDays = theEndOfMonth - DateTime.Now.AddDays(-1).Day;
    var theMonthlyWorkingHoursTimeYesterday = yesterdayWorkingHours - ((monthlyWorkingHours - (totalTime - todayWorkingHours)) / theNumberOfOldRemainingDays);
    var theMonthlyWorkingHoursTimeToday = todayWorkingHours - ((monthlyWorkingHours - totalTime) / theNumberOfRemainingDays);
    var theGoalTimeToday = todayWorkingHours - ((theGoal - totalTime) / theNumberOfRemainingDays);

    var theMonthlyWorkingHoursTodayText = string.Empty;
    var theMonthlyWorkingHoursTimeYesterdayText = string.Empty;
    var theGoalTextToday = string.Empty;
    if(Math.Round(theGoalTimeToday) > 0) 
    {
        theGoalTextToday = "You worked too much :>";
    }
    else if (Math.Round(theGoalTimeToday) == 0)
    {
        theGoalTextToday = "There's nothing for you ^~^";
    }
    else
    {
        theGoalTextToday = "The Missing Time To Reach The Goal Today";
    }

    if(Math.Round(theMonthlyWorkingHoursTimeToday) > 0) 
    {
        theMonthlyWorkingHoursTodayText = "You worked too much :>";
    }
    else if (Math.Round(theMonthlyWorkingHoursTimeToday) == 0)
    {
        theMonthlyWorkingHoursTodayText = "There's nothing for you ^~^";
    }
    else
    {
        theMonthlyWorkingHoursTodayText = "The Missing Time To Reach The Monthly Working Hours Today";
    }

    if(Math.Round(theMonthlyWorkingHoursTimeYesterday) > 0) 
    {
        theMonthlyWorkingHoursTimeYesterdayText = "You worked too much :>";
    }
    else if (Math.Round(theMonthlyWorkingHoursTimeYesterday) == 0)
    {
        theMonthlyWorkingHoursTimeYesterdayText = "There's nothing for you ^~^";
    }
    else
    {
        theMonthlyWorkingHoursTimeYesterdayText = "The Missing Time To Reach The Monthly Working Hours Today";
    }
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine();
    Console.WriteLine($"!----- Information For Total -----!");
    Console.WriteLine($"The Number Of Remaning Days : {theNumberOfRemainingDays}");
    Console.WriteLine($" -------------------- ");
    Console.WriteLine($"The Daily Working Hours (Monthly Working Hours): {monthlyWorkingHours / theEndOfMonth}");
    Console.WriteLine($" -------------------- ");
    Console.WriteLine($"You Should Work Daily That Much According The Number Of Remaining Days (Monthly Working Hours) : {Math.Round((monthlyWorkingHours - totalTime) / theNumberOfRemainingDays, 1, MidpointRounding.AwayFromZero)}");
    Console.WriteLine($" -------------------- ");
    Console.WriteLine($"The Daily Working Hours (The Goal Working Hours): {theGoal / theEndOfMonth}");
    Console.WriteLine($" -------------------- ");
    Console.WriteLine($"You Should Work Daily That Much According The Number Of Remaining Days (The Goal Working Hours) : {Math.Round((theGoal - totalTime) / theNumberOfRemainingDays, 1, MidpointRounding.AwayFromZero)}");
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine();
    Console.WriteLine($"!----- Information For Today And Yesterday -----!");
    Console.WriteLine($" ---------- The Monthly(Today) ---------- ");
    Console.WriteLine($"{theMonthlyWorkingHoursTodayText} : {Math.Round(theMonthlyWorkingHoursTimeToday, 1, MidpointRounding.AwayFromZero)}");
    Console.WriteLine($" ---------- The Monthly(Yesterday) ---------- ");
    Console.WriteLine($"{theMonthlyWorkingHoursTimeYesterdayText} : {Math.Round(theMonthlyWorkingHoursTimeYesterday, 1, MidpointRounding.AwayFromZero)}");
    Console.WriteLine($" ---------- The Goal ---------- ");
    Console.WriteLine($"{theGoalTextToday} : {Math.Round(theGoalTimeToday, 1, MidpointRounding.AwayFromZero)}");
    Console.WriteLine($"!--------------------!");
}
static JsonItem ReadJson(string path, string newPath = "")
{
    try
    {
        string? jsonFile;
        if (newPath == "")
        {
            jsonFile = File.ReadAllText(path);
        }
        else
        {
            jsonFile = File.ReadAllText(newPath);
        }
        return JsonConvert.DeserializeObject<JsonItem>(jsonFile);
    }
    catch(FileNotFoundException fileEx)
    {
        Console.WriteLine($"Could not open file {newPath}");
        return null;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception message : {ex.Message}");
        return null;
    }
}

public class AppSettings
{
    public string JsonFilePath { get; set; }
    public int MonthlyWorkingHours { get; set; }
    public int TheGoal { get; set; }
}
public class JsonItem
{
    public decimal Total { get; set; }
    public decimal TotalRemainingWorkingHours  { get; set; }
    public decimal TotalOvertime { get; set; }
    public IEnumerable<WorkingHours> WorkingHours { get; set; }

}

public class WorkingHours
{
    public string Date { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public decimal TotalTime { get; set; }
    public string IDidWorks { get; set; }
    public bool Status { get; set; }
}