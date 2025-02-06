using TaskTracker.Entities;

namespace TaskTracker.Repositories
{
    public class BoardRepository : RepositoryBase<Board>, IBoardRepository
    {
        public BoardRepository(TodoContext todoContext) : base(todoContext) { }
    }
}