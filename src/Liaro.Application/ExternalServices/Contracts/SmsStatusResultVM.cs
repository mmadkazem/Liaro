namespace Liaro.Application.ViewModels;


public class Return
{
    public int status { get; set; }
    public string? message { get; set; }
}

public class Entries
{
    public int messageid { get; set; }
    public string? message { get; set; }
    public int status { get; set; }
    public string? statusText { get; set; }
    public string? sender { get; set; }
    public string? receptor { get; set; }
    public int date { get; set; }
    public int cost { get; set; }
}

public class SmsResultVM
{
    public virtual Return? @return { get; set; }
    public virtual Entries? entries { get; set; }
}

public class SmsStatusResultVM
{
    public SmsStatusResultVM()
    {
        entries = new List<Entries>();
    }

    public virtual Return? @return { get; set; }
    public virtual List<Entries> entries { get; set; }
}