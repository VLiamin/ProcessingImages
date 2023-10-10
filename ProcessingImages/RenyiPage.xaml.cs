using System.Drawing;

namespace ProcessingImages;

public partial class RenyiPage : ContentPage
{
    private readonly Bitmap image;

    public RenyiPage(Bitmap image)
    {
        this.image = image;

        InitializeComponent();
    }
}