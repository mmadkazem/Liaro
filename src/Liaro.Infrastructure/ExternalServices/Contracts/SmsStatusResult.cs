namespace Liaro.Infrastructure.ExternalServices.Contracts;


public class SmsResult : SmsResultVM
{
    [JsonProperty(PropertyName = "return")]
    public override Return @return { get; set; }
    public override Entries entries { get; set; }
}

public class SmsStatusResult : SmsStatusResultVM
{
    public SmsStatusResult()
    {
        entries = new List<Entries>();
    }

    [JsonProperty(PropertyName = "return")]
    public override Return @return { get; set; }
    public override List<Entries> entries { get; set; }
}