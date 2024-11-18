namespace Shared.Infrastructure.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next();
        var context = new ValidationContext<TRequest>(request);

        var failures = await Validate(validators, context, cancellationToken);
        if (failures.Count > 0) throw new ValidationException(failures);
        return await next();
    }

    private static async Task<List<ValidationFailure>> Validate(IEnumerable<IValidator<TRequest>> validators,
        ValidationContext<TRequest> context, CancellationToken cancellationToken = default)
    {
        var validatorTasks = validators
            .Select(async x => await x.ValidateAsync(context, cancellationToken));

        var validationResults = await Task.WhenAll(validatorTasks);
        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .ToList();

        return failures;
    }
}