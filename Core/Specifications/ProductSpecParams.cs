namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;

        public int PageIndex { get; set; } = 1;

        private int e_pageSize = 6;

        public int PageSize
        {
            get => e_pageSize;
            set => e_pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? BrandId { get; set; }

        public int? TypeId { get; set; }

        public string Sort { get; set; }

        private string e_search;

        public string Search 
        {
            get => e_search;
            set => e_search = value.ToLower();
        }
    }
}