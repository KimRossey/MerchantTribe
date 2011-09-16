update bvc_Product
	set RewriteUrl='products/' + RewriteUrl
GO

update bvc_Category
  set RewriteUrl='categories/' + RewriteUrl WHERE SourceType=0
GO

update bvc_Category
  set RewriteUrl='categories/' + RewriteUrl WHERE SourceType=1
GO

update bvc_Category
  set RewriteUrl='pages/' + RewriteUrl WHERE SourceType=3
GO

update bvc_Category
  set RewriteUrl='filters/' + RewriteUrl WHERE SourceType=4
GO
