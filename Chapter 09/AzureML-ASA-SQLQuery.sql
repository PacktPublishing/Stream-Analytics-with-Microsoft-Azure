WITH subquery AS (  
        SELECT text, sentiment(text) as result from InputData  
    )  

    Select text, result.[Scored Labels]  
    Into output  
    From subquery  