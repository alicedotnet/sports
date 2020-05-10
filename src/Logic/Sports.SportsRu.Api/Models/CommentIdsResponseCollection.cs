using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.SportsRu.Api.Models
{
    public class CommentIdsResponseCollection : List<int>
    {
        public CommentIdsResponseCollection()
        {
        }

        public CommentIdsResponseCollection(IEnumerable<int> collection) : base(collection)
        {
        }
    }
}
