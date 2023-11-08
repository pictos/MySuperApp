namespace MySuperApp;

public class App : Application
{
    protected Window? MainWindow { get; private set; }

    internal static bool IsLoaded { get; private set; }

    internal static Windows.Foundation.Rect Bounds { get; private set; }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
		MainWindow = new Window();
#else
        MainWindow = Microsoft.UI.Xaml.Window.Current;
#endif
        Bounds = MainWindow.Bounds;

        MainWindow.SizeChanged += OnMainWindowSizeChanged;

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (MainWindow.Content is not Frame rootFrame)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            // Place the frame in the current Window
            MainWindow.Content = rootFrame;

            rootFrame.NavigationFailed += OnNavigationFailed;
        }

        if (rootFrame.Content == null)
        {
            // The navigate method is throwing on Windows
            rootFrame.Content = new Shell();


            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            // rootFrame.Navigate(typeof(Shell), args.Arguments);
        }

        // Ensure the current window is active
        MainWindow.Activate();
        IsLoaded = true;
    }

    private void OnMainWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
    {
        Bounds = MainWindow!.Bounds;
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new InvalidOperationException($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
    }
}
