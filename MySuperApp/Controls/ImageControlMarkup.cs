namespace MySuperApp.Controls
{
    public partial class ImageMarkup : Image
    {

        Action<object, ExceptionRoutedEventArgs>? imageFailedAction;
        public ImageMarkup()
        {
            this.Unloaded += (s, e) =>
            {
                this.ImageFailed -= OnImageFailed;
                imageFailedAction = null;
            };
        }



        public ImageMarkup HandleLoadError(Action<object, ExceptionRoutedEventArgs> action)
        { 
            imageFailedAction += action;
            
            return this;
        }
        
        void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            imageFailedAction?.Invoke(sender, e);
        }
    }
}
