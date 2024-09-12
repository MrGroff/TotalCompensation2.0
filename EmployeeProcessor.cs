using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TotalCompensation2._0
{
    public class EmployeeProcessor
    {
        private readonly JsonData _jsonData;

        public EmployeeProcessor(JsonData jsonData)
        {
            _jsonData = jsonData ?? throw new ArgumentNullException(nameof(jsonData));
        }

        // This method prints employee reports as a string
        public string PrintEmployeeReports()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var employee in _jsonData.EmployeeData)
            {
                var report = CalculateEmployeeReport(employee);
                sb.AppendLine(report.ToString());
            }

            return sb.ToString();
        }

        private EmployeeReport CalculateEmployeeReport(EmployeeData employee)
        {
            if (_jsonData.JobMeta == null || !_jsonData.JobMeta.Any())
            {
                throw new Exception("Job meta data is missing or empty.");
            }

            // Ensure unique job meta entries
            var jobMetaLookup = _jsonData.JobMeta
                .GroupBy(j => j.Job)
                .Select(g => g.First())
                .ToDictionary(j => j.Job);

            double totalRegularHours = 0;
            double totalOvertimeHours = 0;
            double totalDoubleTimeHours = 0;
            double totalWage = 0;
            double totalBenefits = 0;

            foreach (var punch in employee.TimePunches)
            {
                if (!jobMetaLookup.TryGetValue(punch.Job, out var jobMeta))
                {
                    throw new Exception($"Job meta not found for job: {punch.Job}");
                }

                double hoursWorked = (punch.End - punch.Start).TotalHours;

                // Calculate hours
                double regularHours = Math.Min(hoursWorked, 8);
                double overtimeHours = Math.Max(0, Math.Min(hoursWorked - 8, 8));
                double doubleTimeHours = Math.Max(0, hoursWorked - 16);

                totalRegularHours += regularHours;
                totalOvertimeHours += overtimeHours;
                totalDoubleTimeHours += doubleTimeHours;

                // Calculate wage
                double wage = (regularHours + 1.5 * overtimeHours + 2 * doubleTimeHours) * jobMeta.Rate;
                totalWage += wage;

                // Calculate benefits
                double benefits = (regularHours + overtimeHours + doubleTimeHours) * jobMeta.BenefitsRate;
                totalBenefits += benefits;
            }

            // Build and return the EmployeeReport object
            return new EmployeeReport
            {
                Employee = employee.Employee,
                RegularHours = totalRegularHours,
                OvertimeHours = totalOvertimeHours,
                DoubleTimeHours = totalDoubleTimeHours,
                WageTotal = totalWage,
                BenefitTotal = totalBenefits
            };
        }
    }

    public class EmployeeReport
    {
        public string Employee { get; set; }
        public double RegularHours { get; set; }
        public double OvertimeHours { get; set; }
        public double DoubleTimeHours { get; set; }
        public double WageTotal { get; set; }
        public double BenefitTotal { get; set; }

        public override string ToString()
        {
            return $@"
Employee: {Employee}
Regular Hours: {RegularHours:F2}
Overtime Hours: {OvertimeHours:F2}
Double Time Hours: {DoubleTimeHours:F2}
Wage Total: {WageTotal:F2}
Benefit Total: {BenefitTotal:F2}
";
        }
    }
}