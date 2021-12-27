<template>
  <satify-modal>
    <template v-slot:header> Edit Product </template>
    <template v-slot:body>
      <ul class="editProduct">
        <li>
          <label for="productName">Name</label>
          <input
            type="text"
            id="productName"
            v-model="editProduct.name"
            @keyup="formChange"
          />
        </li>
        <li>
          <label for="productPrice">Price (CAD)</label>
          <input
            type="number"
            id="productPrice"
            v-model="editProduct.price"
            @keyup="formChange"
          />
        </li>
        <li>
          <label for="idealQuantity">ideal Quantity</label>
          <input
            type="number"
            id="idealQuantity"
            v-model="editProduct.idealQuantity"
            @keyup="formChange"
          />
        </li>
        <li>
          <label>Description</label>
          <textarea
            id="productDesc"
            v-model="editProduct.description"
            @keyup="formChange"
          />
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <satify-button
        type="button"
        @click.native="save"
        aria-label="save edited item"
        :disabled="!isFormDataChange"
      >
        Save Product
      </satify-button>
      <satify-button
        type="button"
        @click.native="close"
        aria-label="close modal"
      >
        Close
      </satify-button>
    </template>
  </satify-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import { IProduct } from "@/types/Product";
import SatifyButton from "@/components/SatifyButton.vue";
import SatifyModal from "@/components/modals/SatifyModal.vue";

@Component({
  name: "EditProductModal",
  components: { SatifyButton, SatifyModal },
})
export default class EditProductModal extends Vue {
  @Prop({ required: true, type: Object as () => IProduct })
  editProduct: IProduct;

  isFormDataChange: boolean = false;

  close() {
    this.$emit("close");
  }

  save() {
    this.$emit("save:edit-product", this.editProduct);
  }

  formChange() {
    this.isFormDataChange = true;
  }
}
</script>

<style scoped lang="scss">
.editProduct {
  list-style: none;
  padding: 0;
  margin: 0;
}

textarea {
  width: 100%;
  height: 200px;
  font-family: Avenir, Helvetica, Arial, sans-serif;
}
</style>
