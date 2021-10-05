namespace H3.Data.Filter
{
    public class ItemMatcher:IMatcher
    {
        private string filed;
        private ComparisonOperatorType comparisonOperatorType;
        private object value;

        public ItemMatcher() { }

        public ItemMatcher(string filed, ComparisonOperatorType comparisonOperatorType, object value, bool isColumn=false)
        {
            this.filed = filed;
            this.comparisonOperatorType = comparisonOperatorType;
            this.value = value;
        }
    }
}