using Core.Base.EF;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ExtensionAndSort;
public static class SortExtensions
{
	public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, SortByEnum? sortBy)
	where T : class, IBaseEntity
	{
		return sortBy switch
		{
			SortByEnum.CreationDate => query.OrderBy(x => x.created_at),
			SortByEnum.CreationDateDescending => query.OrderByDescending(x => x.created_at),
			SortByEnum.updated_ate => query.OrderBy(x => x.updated_at),
			SortByEnum.updated_ateDescending => query.OrderByDescending(x => x.updated_at),
			SortByEnum.slug => query.OrderBy(x => x.slug),
			SortByEnum.slug_descending => query.OrderByDescending(x => x.slug),
			SortByEnum.name => query.OrderBy(x => x.name),
			SortByEnum.name_descending => query.OrderByDescending(x => x.name),
			_ => query.OrderByDescending(x => x.created_at)
		};
	}
}
}
