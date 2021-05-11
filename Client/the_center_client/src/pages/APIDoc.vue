
<template>
    <div></div>
</template>
<script lang="ts">
import { defineComponent, ref } from "vue";
import { GetAPIDoc, onConnected } from "../api/workspace";

export default defineComponent({
    components: {

    },
    props: {

    },
    setup: (prop) => {
        var apidoc = ref<any>(null)
        onConnected(async () => {
            apidoc.value = await GetAPIDoc();
            console.log(apidoc)
        })
        return {
            apidoc
        }
    },
})
</script>
<style>
</style>