using System;
using Eto;
using Eto.Forms;
using Eto.Drawing;
using System.Diagnostics;
using System.IO;

namespace MuGen;
public class FileExplorer : Form
{
	private SelectFolderDialog _openFolder;
	public string folderName = string.Empty;
	public bool folderSelected = false;
	public string[] filePaths = null;

	public FileExplorer()
	{
		this.Visible = false;
		this.Topmost = true;
	}

	public void Open()
	{
		//Eto.Forms.Form placeHolder = new Eto.Forms.Form();
		//placeHolder.Topmost = true;
		_openFolder = new SelectFolderDialog();
		if(_openFolder.ShowDialog(this) == DialogResult.Ok)
		{
			folderName = _openFolder.Directory;
			folderSelected = true;
			filePaths = Directory.GetFiles(folderName, "*.mp3", SearchOption.TopDirectoryOnly);
		}
	}
}
