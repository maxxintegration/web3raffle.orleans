namespace Web3raffle.Utilities.Helpers
{
	public static class FileSystemHelper
	{
		public static DirectoryInfo GetProjectRoot()
		{
			var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

			while (directory != null && !directory.GetFiles("*.sln").Any())
			{
				directory = directory.Parent;
			}

			if (directory is null)
			{
				throw new FileNotFoundException();
			}

			return directory;
		}

		public static List<FileInfo> GetFileFromRoot(params string[] globPatternOrFileName)
		{
			var matcher = new Matcher(StringComparison.OrdinalIgnoreCase);
			matcher.AddIncludePatterns(globPatternOrFileName);

			var result = matcher.Execute(new DirectoryInfoWrapper(GetProjectRoot()));

			if (!result.HasMatches)
			{
				throw new FileNotFoundException();
			}

			return result.Files
				.Select(x => new FileInfo(x.Path))
				.ToList();
		}

		public static string JoinWith(this DirectoryInfo directoryInfo, params string[] paths)
		{
			if (!directoryInfo.Exists)
			{
				throw new FileNotFoundException();
			}

			var pathList = new List<string>
		{
			directoryInfo.FullName
		};

			pathList.AddRange(paths);

			return Path.Join(pathList.ToArray());
		}
	}
}