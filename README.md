## Fiddle

### Token
- Need appsettings.json in project dir to begin per usual
```json
{
  "Discord": {
    "token": "TOKEN_HERE"
  }
}
```
### Commands
``` $ {youtube.com/watch/yourlinkhere}```
>Plays video link. Works with many link formats for youtube. Check regex if it screws up

``` $stop ```
>Stops the currently playing audio

### Starting
- cd into solution directory
```bash
docker compose up -d 
```

#### Errors
- To future me if errors happen for "no" reason
  - Check opus/sodium installations
  - Also the hacky way to pipe two commands in music service
    - Check discord dotnet docs if a better/changed implementation is given with ffmpeg
  - Maybe a 'docker compose up -d --build' with fresh versions of all will fix
