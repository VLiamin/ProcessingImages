using System.Drawing;

namespace ProcessingImages;

public partial class MFSPage : ContentPage
{
	private readonly Bitmap image;

	public MFSPage(Bitmap image)
	{
		this.image = image;

		InitializeComponent();
	}
}