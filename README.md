# cons2db

The idea of this program is to transfer consumption values that are stored in a text file to a relational database.

Examples of consumption data:

- Network usage: produced for example by vnstat
- Electrical power (generated/consumed): produced for example my metering devices like smart meters
- Gas (fed/taken)

If data is stored in a relational database, it can be easily analyzed. Questions like the following might be answered:

- At which time of the day occured the highest consumption in average?
- At which day of the week occured the lowest consumption in average?
- How was the average development during a weekday specified?

# How does it work

The program reads in data from a file and puts it into the database.

# Implementations

By now, the program can read in XML data produced by vnstat and write this data to a PostgreSQL database.