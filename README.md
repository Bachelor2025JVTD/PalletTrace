
# PalletTrace

The program shows the conecept to trace the positioning of a pallet with use of a camera, two RFID antennas and two reciver.





## Installation

Install PalletTrace with terminal:

```bash
  git clone https://github.com/Bachelor2025JVTD/PalletTrace.git
```


    
## Configuration

The hardware devices used:
- Logitech Carl Zeiss HD c930e 1080p.
- Parallax 28340 USB 125 kHz. 
- Two RFID tags of 125 kHz.


#### Configuration file
All settings to be changed are set in app.config file.

- Set "tolerance" to adjust the sensitivety.
- Set "targetHeight" to the length of the pallet.
- Set "targetWidth" to the width of the pallet.

#### Database
Link to the database structure: [Database structure.sql](https://github.com/Bachelor2025JVTD/PalletTrace/blob/master/Database/QUERY_PALLET_TRACE.sql)
