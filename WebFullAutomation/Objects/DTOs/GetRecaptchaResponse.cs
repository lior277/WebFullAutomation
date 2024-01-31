namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetRecaptchaResponse
    {
        public string _id { get; set; }
        public bool recaptcha_enabled { get; set; }
        public string recaptcha_site_key { get; set; }

    }
}
