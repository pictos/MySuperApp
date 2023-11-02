namespace MySuperApp.Pages
{

    readonly record struct SearchResult(string Title, string ImageUrl);
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.Content
            (
                MainContent()
            );
        }


        Grid MainContent()
        {
            return new Grid().RowDefinitions("*, auto, *")
                .Children
                (
                    new TextBox().PlaceholderText("Search...")
                        .Grid(0, 0),
                    new ListView().ItemTemplate<SearchResult>
                    (result =>
                        new Grid().Children
                        (
                            new Image().Source( x=> )
                            new TextBlock()
                        ).RowDefinitions("* , auto")
                    )

                );
        }
    }
}
