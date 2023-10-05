# Feature Toggles API

## Introduction

This API is used by all customers to manage their the toggles they create for their applications.

### What is a feature toggle?

A feature is a piece of work that you want to implement in an application e.g. in a online webstore you might want an experimental payment page. It would differ from the existing one because it would offer different payment methods. We bring in toggles to allow you show or hide your toggle based on the environment you're on i.e. Test or Production.

## Build

dotnet build

## Run

dotnet run

## Test

dotnet test <project-name>

### Integration

#### Pre-requisite

To run these integration tests you need to install Docker Desktop. See why below.

#### Faking DB interactions

We integrate .NET's `WebApplicationFactory` with `Testcontainers.MsSql` NuGet package to spin up a real, temporary MSSQL database in Docker. This allows us to interact with a real DB server environment but not affect real data or hold onto a database just for testing. The library creates containers with a DB for each test class, runs them in parallel and then destroys them after they're done.
