<script lang="ts">
  import WindowControls from '$components/WindowControls.svelte';
  import { photino } from '$stores';

  const { io, os, window } = $photino;

  async function exec<T>(func: () => Promise<T>): Promise<T> {
    try {
      return await func();
    } catch (error) {
      return error.toString();
    }
  }

  let cd: string;
  async function currentDir() {
    cd = 'Getting current directory...';
    cd = await exec(() => io.cwd());
  }

  let files: string[] | string;
  async function listFiles() {
    files = 'Gettings list of files...';
    const cwd = await exec(() => io.cwd());
    files = (await exec(() => io.listFiles(cwd))).join('<br>');
  }

  let folders: string[] | string;
  async function listFolders() {
    folders = 'Gettings list of folders...';
    const cwd = await exec(() => io.cwd());
    folders = (await exec(() => io.listFolders(cwd))).join('<br>');
  }

  
</script>

<main>
  <WindowControls />

  <div>
    <button on:click={currentDir}>Current Directory</button>
    <div>{cd ?? 'Click "Current Directory"'}</div>
  </div>

  <div>
    <button on:click={listFiles}>List Files</button>
    <div>{@html files ?? 'Click "List Files"'}</div>
  </div>

  <div>
    <button on:click={listFolders}>List Folders</button>
    <div>{@html folders ?? 'Click "List Folders"'}</div>
  </div>
</main>

<style lang="scss">
  main {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    padding: 0.5rem;
    height: 100vh;
    width: 100vw;
    overflow-x: hidden;
  }
</style>
