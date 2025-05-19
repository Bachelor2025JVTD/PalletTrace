
# PalletTrace

The program demonstrates the concept of tracking the position of a pallet using a camera, two RFID tags, and a receiver.




## Installation

Install PalletTrace with terminal:

```bash
  git clone https://github.com/Bachelor2025JVTD/PalletTrace.git
```


    
## Configuration

Hardware devices used:
- Logitech Carl Zeiss HD c930e 1080p.
- Parallax 28340 USB 125 kHz. 
- Two RFID tags of 125 kHz.
- 3D-printed pallet


#### Configuration file
All settings are defined in the app.config file.

- Set "connectionString" to connect to the database.
- Set "tolerance" to adjust the sensitivity.
- Set "targetHeight" to the height of the pallet.
- Set "targetWidth" to the width of the pallet.

#### Database
Link to the database structure: [Database structure.sql](https://github.com/Bachelor2025JVTD/PalletTrace/blob/master/Database/QUERY_PALLET_TRACE.sql)
