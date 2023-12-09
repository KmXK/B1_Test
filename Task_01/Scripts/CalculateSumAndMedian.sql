create procedure [dbo].[CalculateSumAndMedian]
    as
begin
    declare @sum bigint;
    SET @sum = (
        SELECT SUM(CAST([EvenInteger] AS bigint))
        FROM [dbo].[FilesData]
    );
    
    declare @median float;
    with [ordered_data] as (
        select
            [RandomDouble],
            row_number() over (order by [RandomDouble]) as row_asc,
                row_number() over (order by [RandomDouble] desc) as row_desc
        from [dbo].[FilesData]
    )
    select
        @median = avg(RandomDouble)
    from
        ordered_data
    where
        row_asc = row_desc or row_asc + 1 = row_desc or row_asc = row_desc + 1;
    
    select @sum as [Sum], @median as [Median];
end;