using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tree.Controllers
{
    public class TreeController : Controller
    {
        ApplicationDbContext _context;
        public TreeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateNode")]
        public async Task<IActionResult> CreateNode([FromBody] NodeModel model)
        {
            try
            {
                NodeEntity entity = new NodeEntity();
                entity.Data = model.Data;
                entity.Id = model.Id;
                entity.ParentId = model.ParentId;
                await _context.Tree.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Ok(entity);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
        [HttpGet("GetTree")]
        public async Task<IActionResult> GetTree()
        {
            List<NodeEntity> all = await _context.Tree.ToListAsync();
            var root = all.GenerateTree(c => c.Id, c => c.ParentId);
            return Ok(all.Where(x => x.ParentId == null));
        }
        [HttpGet("GetNodeById/{id}")]
        public async Task<IActionResult> GetNodeById(int id)
        {
            NodeEntity entity = await _context.Tree.FindAsync(id);
            return Ok(entity);
        }

        [HttpPut("UpdateNode")]
        public async Task<IActionResult> UpdateNode([FromBody] NodeModel model)
        {
            var entity = await _context.Tree.FindAsync(model.Id);
            if (entity == null)
                return BadRequest();
            entity.ParentId = model.ParentId;
            entity.Data = model.Data;
            await _context.SaveChangesAsync();

            List<NodeEntity> all = await _context.Tree.ToListAsync();
            var root = all.GenerateTree(c => c.Id, c => c.ParentId);
            return Ok(all.Where(x => x.ParentId == null));
        }
    }

    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
    internal static class GenericHelpers
    {
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> id_selector,
            Func<T, K> parent_id_selector,
            K root_id = default(K))
        {
            foreach (var c in collection.Where(c => EqualityComparer<K>.Default.Equals(parent_id_selector(c), root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }
    }


}
