namespace ABCMoneyTransfer.App.Models
{
    public class Currency
    {
        public string Iso3 { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Unit { get; set; } 
    }

    public class Data
    {
        public List<Payload> Payload { get; set; } = [];
    }

    public class Errors
    {
        public object? Validation { get; set; }
}

    public class Links
    {
        public object? Prev { get; set; }
        public object? Next { get; set; }
    }

    public class Pagination
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public Links? Links { get; set; }
    }

    public class Params
    {
        public object? Date { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public object? PostType { get; set; }
        public string PerPage { get; set; } = string.Empty;
        public string Page { get; set; } = string.Empty;
        public object? Slug { get; set; }
        public object? Q { get; set; }
    }

    public class Payload
    {
        public string Date { get; set; } = string.Empty;
        public string PublishedOn { get; set; } = string.Empty;
        public string ModifiedOn { get; set; } = string.Empty;
        public List<Rate> Rates { get; set; } = [];
    }

    public class Rate
    {
        public Currency Currency { get; set; } = new Currency();
        public string Buy { get; set; } = string.Empty;
        public string Sell { get; set; } = string.Empty;
    }

    public class Root
    {
        public Status Status { get; set; } = new Status();
        public Errors Errors { get; set; } = new Errors();
        public Params Params { get; set; } = new Params();
        public Data Data { get; set; } = new Data();
        public Pagination Pagination { get; set; } = new Pagination();
    }

    public class Status
    {
        public int Code { get; set; }
    }

}
