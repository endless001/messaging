using Messaging.Domain.AggregatesModel.NotificationAggregate;
using Messaging.Infrastructure.EntityConfigurations;

namespace Messaging.Infrastructure;

public class MessagingContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;
    public DbSet<Message> Messages { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<TemplateType> TemplateTypes { get; set; }
    public DbSet<TemplateProperty> TemplateProperties { get; set; }
    public DbSet<Notification> Notifications  { get; set; }
    public MessagingContext(DbContextOptions<MessagingContext> options) : base(options)
    {
    }

    public MessagingContext(DbContextOptions<MessagingContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        System.Diagnostics.Debug.WriteLine("MessagingContext::ctor ->" + GetHashCode());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationEntityTypeConfiguration());
    }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;



    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {

        await _mediator.DispatchDomainEventsAsync(this);

        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}

public class MessagingContextDesignFactory : IDesignTimeDbContextFactory<MessagingContext>
{
    public MessagingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MessagingContext>()
            .UseMySql(MySqlServerVersion.AutoDetect(
                "Server=localhost;Port=5432;Database=messaging;User Id=postgres;Password=postgres;"));

        return new MessagingContext(optionsBuilder.Options);
    }
}