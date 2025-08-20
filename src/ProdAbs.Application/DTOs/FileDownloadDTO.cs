namespace ProdAbs.Application.DTOs;

public class FileDownloadDTO
{
    public Stream File { get; set; }

    public string FileName { get; set; }

    public string ContentType { get; set; }
}