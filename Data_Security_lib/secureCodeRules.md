# Security Code Rules
=======================
1. Login Error/Warning messages cannot be very specific, not leaving password hint for hackers;
2. To avoid Brute Force attack - Must have password attempts threshold in login check code(3 failed attempts, lock account 15 minutes)
3. To avoid XSS - Input Filtering (WhiteListing more secure than blacklisting) & Output Encoding ('<' as '<b>' OR '&lt')
