namespace SkinkiDriverApi.ValueObjects
{
    public class PageResult
    {
        /// <summary>
        /// Quantidade por página
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Página atual
        /// </summary>
        public int Offset { get; set; }
      
        /// <summary>
        /// Total de registros
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Total de páginas
        /// </summary>
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalCount / (double)Limit);
            }
        }

        /// <summary>
        /// Página anterior
        /// </summary>
        public int PreviousPage
        {
            get
            {
                return Offset > 1 ? Offset - 1 : Offset;
            }
        }

        /// <summary>
        /// Próxima página
        /// </summary>
        public int NextPage
        {
            get
            {
                return Offset < TotalPages ? Offset + 1 : Offset;
            }
        }
    }
}
