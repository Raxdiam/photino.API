<script lang="ts">
  import { Photino } from 'photino.js';

  const { window, io } = Photino;

  let fileList: string[] = [];
  let filePath: string;
  let fileContents: string = '';

  async function getFileList() {
    fileList = (await io.listFiles('.', '*', true)).filter((f) =>
      f.endsWith('.txt') ||
      f.endsWith('.json') ||
      f.endsWith('.html') ||
      f.endsWith('.js') ||
      f.endsWith('.css'));
  }

  async function readFile() {
    fileContents = await io.readFileText(filePath);
  }

  getFileList();
</script>

<main class="flex col">
  <div class="flex">
    <button on:click={async () => await window.setMinimized(!(await window.getMinimized()))}>Minimize</button>
    <button on:click={async () => await window.setMaximized(!(await window.getMaximized()))}>Maximize</button>
    <button on:click={async () => await window.close()}>Close</button>
    <button on:click={async () => await window.setContextMenuEnabled(!(await window.getContextMenuEnabled()))}>Context Menu</button>
    <button on:click={async () => await window.setDevToolsEnabled(!(await window.getDevToolsEnabled()))}>Dev Tools</button>
    <button on:click={async () => await window.setTitle((await window.getTitle()) + '*')}>Title</button>
    <button on:click={async () => await window.setTopMost(!(await window.getTopMost()))}>Top-Most</button>
    <button on:click={async () => await window.setFullScreen(!(await window.getFullScreen()))}>Full Screen</button>
  </div>

  <br>

  <div class="flex col">
    <div class="flex">
      <button on:click={readFile}>Read File</button>
      <select bind:value={filePath}>
        {#each fileList as file}
          <option>{file}</option>
        {/each}
      </select>
      <button on:click={getFileList}>Refresh</button>
    </div>
    <code>{fileContents}</code>
  </div>
</main>
