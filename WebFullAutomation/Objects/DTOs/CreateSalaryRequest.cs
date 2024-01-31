namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateSalaryRequest
    {
        public string name { get; set; }
        public January january { get; set; }
        public February february { get; set; }
        public March march { get; set; }
        public April april { get; set; }
        public May may { get; set; }
        public June june { get; set; }
        public July july { get; set; }
        public August august { get; set; }
        public September september { get; set; }
        public October october { get; set; }
        public November november { get; set; }
        public December december { get; set; }
    }

    public class January
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class Deposit_Count
    {
        public int target { get; set; }
        public int fail { get; set; }
        public int success { get; set; }
    }

    public class Deposit_Sum
    {
        public int target { get; set; }
        public int fail { get; set; }
        public int success { get; set; }
    }

    public class Conversion
    {
        public int target { get; set; }
        public int fail { get; set; }
        public int success { get; set; }
    }

    public class February
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class March
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class April
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class May
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class June
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class July
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class August
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class September
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class October
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class November
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }

    public class December
    {
        public Deposit_Count deposit_count { get; set; }
        public Deposit_Sum deposit_sum { get; set; }
        public Conversion conversion { get; set; }
    }
}
