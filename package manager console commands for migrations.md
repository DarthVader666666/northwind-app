# Package Manager Console Commands for Migrations     

Migration commands in Entity Framework Core can be executed using the Package Manager Console in Visual Studio. Open the Package Manager Console from menu Tools -> NuGet Package Manger -> Package Manager Console in Visual Studio to execute the following commands.

| PMC Command | Usage |
| ------ | ------ |
| Get-Help entityframework | Displays in formation about entity framework commands. |
| Add-Migration <migration name> | Creates a migration by adding a migration snapshot. |
| Remove-Migration | Removes the last migration snapshot. |
| Add-Migration <migration name> | Creates a migration by adding a migration snapshot. |
| Update-Database | Updates the database schema based on the last migration snapshot. |	
| Script-Migration | Generates a SQL script using all the migration snapshots. |	
| Scaffold-DbContext | Generates a DbContext and entity type classes for a specified database. This is called reverse engineering. |	
| Get-DbContext | Gets information about a DbContext type. |	
| Drop-Database | Drops the database. |		
	
## Get-Help
PM> get-help entityframework

      
                     _/\__
               ---==/    \\
         ___  ___   |.    \|\
        | __|| __|  |  )   \\\
        | _| | _|   \_/ |  //|\\
        |___||_|       /   \\\/\\

TOPIC
    about_EntityFrameworkCore

SHORT DESCRIPTION
    Provides information about the Entity Framework Core Package Manager Console Tools.

LONG DESCRIPTION
    This topic describes the Entity Framework Core Package Manager Console Tools. 
    See https://docs.efproject.net for information on Entity Framework Core.

    The following Entity Framework Core commands are available.

        Cmdlet                      Description
        --------------------------  ---------------------------------------------------
        Add-Migration               Adds a new migration.

        Drop-Database               Drops the database.

        Get-DbContext               Gets information about a DbContext type.

        Remove-Migration            Removes the last migration.

        Scaffold-DbContext          Scaffolds a DbContext and entity types for a database.

        Script-Migration            Generates a SQL script from migrations.

        Update-Database             Updates the database to a specified migration.

SEE ALSO   
    Add-Migration     
    Drop-Database    
    Get-DbContext    
    Remove-Migration    
    Scaffold-DbContext   
    Script-Migration    
    Update-Database   
        
## Add-Migration
NAME
    Add-Migration
    
SYNOPSIS
    Adds a new migration.
    
    
SYNTAX
    Add-Migration [-Name] <String> [-OutputDir <String>] [-Context <String>] [-Project <String>] 
                    [-StartupProject <String>] [<CommonParameters>]
    
    
DESCRIPTION
    Adds a new migration.

REMARKS
    To see the examples, type: "get-help Add-Migration -examples".
    For more information, type: "get-help Add-Migration -detailed".
    For technical information, type: "get-help Add-Migration -full".
    
## Remove-Migration
NAME
    Remove-Migration
    
SYNOPSIS
    Removes the last migration.
    
SYNTAX
    Remove-Migration [-Force] [-Context <String>] [-Project <String>] [-StartupProject <String>] 
                        [<CommonParameters>]
    
DESCRIPTION
    Removes the last migration.

RELATED LINKS
    Add-Migration
    about_EntityFrameworkCore 

REMARKS
    To see the examples, type: "get-help Remove-Migration -examples".
    For more information, type: "get-help Remove-Migration -detailed".
    For technical information, type: "get-help Remove-Migration -full".
 
## Update-Database
NAME
    Update-Database
    
SYNOPSIS
    Updates the database to a specified migration.
    
    
SYNTAX
    Update-Database [[-Migration] <String>] [-Context <String>] [-Project <String>] 
                        [-StartupProject <String>] [<CommonParameters>]
    
    
DESCRIPTION
    Updates the database to a specified migration.
    

RELATED LINKS
    Script-Migration
    about_EntityFrameworkCore 

REMARKS
    To see the examples, type: "get-help Update-Database -examples".
    For more information, type: "get-help Update-Database -detailed".
## For technical information, type: "get-help Update-Database -full".
Script-Migration
NAME
    Script-Migration
    
SYNOPSIS
    Generates a SQL script from migrations.
    
    
SYNTAX
    Script-Migration [-From] <String> [-To] <String> [-Idempotent] [-Output <String>] 
                        [-Context <String>] [-Project <String>] [-StartupProject <String>] 
                        [<CommonParameters>]
    
    Script-Migration [[-From] <String>] [-Idempotent] [-Output <String>] [-Context <String>] 
                        [-Project <String>] [-StartupProject <String>] [<CommonParameters>]
    
    
DESCRIPTION
    Generates a SQL script from migrations.
    

RELATED LINKS
    Update-Database
    about_EntityFrameworkCore 

REMARKS
    To see the examples, type: "get-help Script-Migration -examples".
    For more information, type: "get-help Script-Migration -detailed".
    For technical information, type: "get-help Script-Migration -full".

## Scaffold-Dbcontext
NAME
    Scaffold-DbContext
    
SYNOPSIS
    Scaffolds a DbContext and entity types for a database.
    
    
SYNTAX
    Scaffold-DbContext [-Connection] <String> [-Provider] <String> [-OutputDir <String>] 
                        [-Context <String>] [-Schemas <String[]>] [-Tables <String[]>] 
                        [-DataAnnotations] [-Force] [-Project <String>] [-StartupProject <String>] 
                        [<CommonParameters>]
    
    
DESCRIPTION
    Scaffolds a DbContext and entity types for a database.
    

RELATED LINKS
    about_EntityFrameworkCore 

REMARKS
    To see the examples, type: "get-help Scaffold-DbContext -examples".
    For more information, type: "get-help Scaffold-DbContext -detailed".
    For technical information, type: "get-help Scaffold-DbContext -full".

## Get-DbContext
NAME
    Get-DbContext
    
SYNOPSIS
    Gets information about a DbContext type.
    
    
SYNTAX
    Get-DbContext [-Context <String>] [-Project <String>] [-StartupProject <String>] 
                    [<CommonParameters>]
    
    
DESCRIPTION
    Gets information about a DbContext type.
    

RELATED LINKS
    about_EntityFrameworkCore 

REMARKS
    To see the examples, type: "get-help Get-DbContext -examples".
    For more information, type: "get-help Get-DbContext -detailed".
    For technical information, type: "get-help Get-DbContext -full".

## Drop-Database
NAME
    Drop-Database
    
SYNOPSIS
    Drops the database.
    
    
SYNTAX
    Drop-Database [-Context <String>] [-Project <String>] [-StartupProject <String>] 
                    [-WhatIf] [-Confirm] [<CommonParameters>]
    
    
DESCRIPTION
    Drops the database.
    

RELATED LINKS
    Update-Database
    about_EntityFrameworkCore 

REMARKS
    To see the examples, type: "get-help Drop-Database -examples".
    For more information, type: "get-help Drop-Database -detailed".
    For technical information, type: "get-help Drop-Database -full".
