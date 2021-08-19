<script lang="ts">
  import { photino } from '$stores';

  export let text: string;
  export let defaultMessage: string;
  export let ns: string;
  export let action: string;
  export let params: any[] = null;
  export let messageProperty: string = null;

  let message: string;

  async function handleAction() {
    try {
      console.log(params)
      const result = await $photino[ns][action](...params);
      console.log(result);
      if (messageProperty === null)
        message = result;
      else
        message = result[messageProperty];
    } catch (error) {
      message = error.toString();
    }
  }
</script>

<button on:click={handleAction}>{text}</button>
<div>{message ?? defaultMessage}</div>

<style lang="scss">
</style>
