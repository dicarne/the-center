<template>
    <div class="main-work-area">
        <Button @click="getboard()">刷新</Button>
    </div>

    <Row :gutter="[16, 16]">
        <Col :span="6">
            <div class="card-board card-body">
                <a-button @click="createBoard">+</a-button>
            </div>
        </Col>
        <Col v-for="item in list" :key="item.id" :span="item.w">
            <div class="card-board card-body">
                <BoardElement
                    v-for="ui in item.uIComs"
                    :key="ui.id"
                    :ui="ui"
                    :workspace="workspace"
                    :board="item.id"
                />
            </div>
        </Col>
    </Row>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { Row, Col, Button } from "ant-design-vue";
import { BoardUI, CreateBoard, GetBoards, onConnected } from "../api/workspace";
import BoardElement from "./BoardElement.vue"

export default defineComponent({
    components: {
        Row,
        Col,
        Button,
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

<style>
.main-work-area {
    padding-bottom: 10px;
}
.card-body{
    background-color: #fff;
    padding: 24px;
}
.card-board {
    border-radius: 8px;
    box-shadow: 0px 0px 10px #e6e6e6;
}
.card-board:hover {
    box-shadow: 0px 0px 10px #d3d3d3;
}
</style>
