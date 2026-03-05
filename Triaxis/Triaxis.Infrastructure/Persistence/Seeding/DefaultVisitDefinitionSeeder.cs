using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Triaxis.Domain.Common.Enums;
using Triaxis.Domain.Entities;

namespace Triaxis.Infrastructure.Persistence.Seeding;

public class DefaultVisitDefinitionSeeder : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultVisitDefinitionSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TriaxisDbContext>();

        if (await context.VisitDefinitions.AnyAsync(v => v.IsDefault, cancellationToken))
            return;

        var defaults = new (string Name, string Code, VisitType Type, int Order)[]
        {
            ("Prescreening", "PSCR", VisitType.Prescreening, 1),
            ("Screening", "SCR", VisitType.Screening, 2),
            ("Rescreening", "RSCR", VisitType.Rescreening, 3),
            ("Enrollment", "ENR", VisitType.Enrollment, 4),
            ("Scheduled Visit", "SCHED", VisitType.ScheduledVisit, 5),
            ("Unscheduled Visit", "UNSCHED", VisitType.UnscheduledVisit, 6),
            ("Early Termination", "ET", VisitType.EarlyTermination, 7),
            ("Completion", "COMP", VisitType.Completion, 8),
            ("Drug Replacement", "DREP", VisitType.DrugReplacement, 9),
            ("Drug Safety Unblinding", "DSU", VisitType.DrugSafetyUnblinding, 10),
        };

        foreach (var (name, code, type, order) in defaults)
        {
            context.VisitDefinitions.Add(VisitDefinition.CreateDefault(name, code, type, order));
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
