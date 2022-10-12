def crop_row_border_single(colour_row: list) -> list:
    """Return a colour row with the colours from the given colour_row, except with the first and last colour removed.

    You may ASSUME that:
    - colour_row is a valid colour row (i.e., is a list of RGB tuples)
    - len(colour_row) >= 2

example_colours = [[0, 255, 200], [1, 2, 3], [100, 100, 100], [181, 57, 173], [33, 0, 197]]
    >>> crop_row_border_single(example_colours)
    [[1, 2, 3], [100, 100, 100], [181, 57, 173]]

    You may implement this function by using a list comprehension OR by calling crop_row
    with the appropriate arguments. (For extra practice, try both ways!)
    """
    single = [x for x in colour_row[1:-1]]
    # single = colour_row
    
    # single = [];
    return single

print(crop_row_border_single("1111"))

def fade_row(colour_row: list) -> list:
    return [[colour_row[x][0]*(x+1)/5, colour_row[x][1]*(x+1)/5, colour_row[x][2]*(x+1)/5] for x in range(len(colour_row))]

colour_row = [[0, 255, 200], [1, 2, 3], [100, 100, 100], [181, 57, 173], [33, 0, 197]]
print(fade_row(colour_row))