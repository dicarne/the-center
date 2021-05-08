<template>
    <Button @click="getboard()">刷新</Button>
    <Row>
        <Col>
            <a-button @click="createBoard">+</a-button>
        </Col>
        <Col v-for="item in list" :key="item.id" :span="item.w">
            <Card>
                <BoardElement v-for="ui in item.uIComs" :key="ui.id" :ui="ui" :workspace="workspace" :board="item.id"/>
            </Card>
        </Col>
    </Row>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { Row, Col, Button, Card } from "ant-design-vue";
import { BoardUI, CreateBoard, GetBoards, onConnected } from "../api/workspace";
import BoardElement from "./BoardElement.vue"

export default defineComponent({
    components: {
        Row,
        Col,
        Button,
        Card,
        BoardElement
    },
    props: {
        workspace: {
            type: String,
            validator(this: void, v: string) {
                return !!v;
            },
            required: true,
        },
    },
    setup: (prop) => {
        const list = ref([] as BoardUI[]);
        const getboard = async () => {
            list.value = await GetBoards(prop.workspace);
        };
        onConnected(() => getboard());
        const createBoard = async () => {
            await CreateBoard(prop.workspace, "runscript")
            await getboard()
        }
        return { getboard, list, createBoard, workspace: prop.workspace };
    },
});
</script>

<style></style>
