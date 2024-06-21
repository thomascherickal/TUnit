﻿using FluentAssertions;
using ModularPipelines.Context;

namespace TUnit.Pipeline.Modules.Tests;

public class TimeoutTests1 : TestModule
{
    protected override async Task<TestResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var start = DateTime.UtcNow;
        return await RunTestsWithFilter(context, 
            "/*/*/TimeoutCancellationTokenTests/BasicTest",
            [
                result => result.Successful.Should().BeTrue(),
                result => result.Total.Should().Be(1),
                result => result.Passed.Should().Be(0),
                result => result.Failed.Should().Be(1),
                result => result.Skipped.Should().Be(0),
                _ => (DateTime.UtcNow - start).Should().BeLessThan(TimeSpan.FromMinutes(1)),
                _ => (DateTime.UtcNow - start).Should().BeGreaterThan(TimeSpan.FromSeconds(30)),
            ], cancellationToken);
    }
}