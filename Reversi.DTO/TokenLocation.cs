namespace Reversi.DTO
{
    public struct TokenLocation
    {
        public TokenLocation(int i_RowN, int i_ColN)
        {
            ColN = i_ColN;
            RowN = i_RowN;
        }

        public readonly int ColN;
        public readonly int RowN;
    }
}
