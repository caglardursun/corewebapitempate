using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;

namespace API.Data
{
    /// <summary>
    /// Sunucu tarafında serverside pagination için gerekli parametreleri içerir
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class UrlQueryParameters
    {
        private const int maxPageSize = 100;
        private int _pageSize = 20;
        /// <summary>
        /// Kaçıncı Sayfa Olduğu
        /// </summary>
        /// <value></value>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// Sayfada kaç kayıt gösterileceği
        /// </summary>
        /// <value></value>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        /// <summary>
        /// Müşteri ID si 
        /// müşteri admin yetkili ise tüm kayıtlar gelir
        /// </summary>
        /// <value></value>
        public long? ReferenceID { get; set; }
        /// <summary>
        /// Toplam kayıt sayısı
        /// </summary>
        /// <value></value>
        public bool IncludeCount { get; set; } = false;
    }

    /// <summary>
    /// <see cref="Column"/>
    /// <see cref="SearchValue"/>
    /// </summary>
    public class SearchField
    {
        /// <summary>
        /// Tablo yada alan ismi
        /// </summary>
        /// <value></value>
        public string Column { get; set; }

        /// <summary>
        /// Like ile aranacak keyword. 
        /// </summary>
        /// <value></value>
        public string SearchValue { get; set; }        

    }

    /// <summary>
    /// Dinamik filtre sistemi. Tablo isimleri 
    /// Bknz : <see cref="SearchField"/>
    /// </summary>
    public class UrlQuerySearchParameters : UrlQueryParameters
    {
        public UrlQuerySearchParameters()
        {
            SearchFields = new List<SearchField>();
        }
        public IList<SearchField> SearchFields { get; set; }
    }
}
