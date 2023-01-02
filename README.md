# WorkingHoursConsole
**It's calculating your total working hours you added it on Buddee.**

# Usage
**appsettings.json**
```yaml
{
  "JsonFilePath": "D:\\Working Hours\\2023\\working-hours-january.json",
  "MonthlyWorkingHours": 160,
  "TheGoal": 250
}
```

**workinghours.json**
```yaml
{
  "Total": 5.0,
  "TotalRemainingWorkingHours": 155,
  "TotalOvertime": 0.0,
  "WorkingHours": [
    {
      "Date": "01-01-2023",
      "Start": "08:00",
      "End": "13:00",
      "TotalTime": 5.00,
      "IDidWorks": "note your stuff about the task",
      "Status": true
    }
  ]
}
```

## Descriptions 
**appsettings.json parameters**
1. JsonFilePath = your working hours json file path
2. MonthlyWorkingHours = the working hours you should work
3. TheGoal = if you have overtime goal, fill it or just leave it as 0

**workinghours.json parameters**
1. Total = auto calculating based on your working hours sum
2. TotalRemainingWorkingHours = auto calculating based on your working hours sum and 'MonthlyWorkingHours'
3. TotalOvertime = auto calculating if your working hours sum's higher than 'MonthlyWorkingHours'
4. WorkingHours = it's a list of Json Item
   - Date = it'll be useful at the future
   - Start = start time of working hour
   - End = end time of working hour 
   - TotalTime = the total time of buddee (it'll be auto calcuted at the future)
   - IDidWorks = note your stuff about the task
   - Status = if it has been created or not (it'll be depends on auto adding at the future)
