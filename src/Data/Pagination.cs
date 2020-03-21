using System;

namespace API.Data
{
    /// <summary>
    /// API servislerinde response header içinde bulunan toplam kayıt ve sayfa bilgileri
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// Kaçıncı sayfa olduğu
        /// </summary>
        /// <value></value>
        public int PageNumber { get; set; }
        /// <summary>
        /// Kayıt Sayısı
        /// </summary>
        /// <value></value>
        public int PageSize { get; set; }
        /// <summary>
        /// Toplam kayıt sayısı
        /// </summary>
        /// <value></value>
        public int? TotalRecords { get; set; }
        public int? TotalPages => TotalRecords.HasValue ? (int)Math.Ceiling(TotalRecords.Value / (double)PageSize) : (int?)null;
        /// <summary>
        /// Sonraki Sayfa
        /// </summary>
        public bool HasPrevious => PageNumber > 1;
        /// <summary>
        /// Önceki Sayfa
        /// </summary>
        public bool HasNext => PageNumber < TotalPages;
    }
}
