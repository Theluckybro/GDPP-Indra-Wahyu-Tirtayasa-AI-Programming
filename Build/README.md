# House of Silence — Windows Player Build

## How to run

1. Make sure Git LFS is installed and the binaries are downloaded:

   ```
   git lfs install
   git lfs pull
   ```

2. Double-click **`Join-Build.bat`** in this folder and wait for it to finish.
3. Run **`House of Silence.exe`**.

Step 2 only has to be done once, right after cloning.

## Why the extra step

`House of Silence_Data/sharedassets1.assets.resS` is 2,262,637,984 bytes
(2157.82 MiB). Git LFS on GitHub rejects any single file larger than
2,147,483,648 bytes (2048 MiB), so that one file is committed as three parts:

| Part | Size |
| --- | --- |
| `sharedassets1.assets.resS.001` | 1024 MiB |
| `sharedassets1.assets.resS.002` | 1024 MiB |
| `sharedassets1.assets.resS.003` | 109.82 MiB |

`Join-Build.bat` concatenates them back into the original file, which is
listed in `.gitignore` so it never gets committed. Every other file in this
folder is the untouched Unity build output.

To verify the join, the original file's SHA-256 is:

```
cd8bac8a54d0cf3bc7386fdff7369558bfbde739b08936726db5f4b234852d95
```

```
certutil -hashfile "House of Silence_Data\sharedassets1.assets.resS" SHA256
```

On Linux or macOS, join the parts with:

```
cat "House of Silence_Data/sharedassets1.assets.resS."{001,002,003} \
  > "House of Silence_Data/sharedassets1.assets.resS"
```
