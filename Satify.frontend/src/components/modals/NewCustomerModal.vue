<template>
  <satify-modal>
    <template v-slot:header> Add New Customer </template>
    <template v-slot:body>
      <ul class="newCustomer">
        <li>
          <label for="firstName">First Name</label>
          <input type="text" id="firstName" v-model="customer.firstName" />
        </li>
        <li>
          <label for="lastName">Last Name</label>
          <input type="text" id="lastName" v-model="customer.lastName" />
        </li>
        <li>
          <label for="address1">Address Line 1</label>
          <input
            type="text"
            id="address1"
            v-model="customer.primaryAddress.addressLine1"
          />
        </li>
        <li>
          <label for="city">City</label>
          <input type="text" id="city" v-model="customer.primaryAddress.city" />
        </li>
        <li>
          <label for="postalCode">Postal Code</label>
          <input
            type="text"
            id="postalCode"
            v-model="customer.primaryAddress.postalCode"
          />
        </li>
        <li>
          <label for="province">Province</label>
          <input
            type="text"
            id="province"
            v-model="customer.primaryAddress.province"
          />
        </li>
        <li>
          <label for="country">Country</label>
          <input
            type="text"
            id="country"
            v-model="customer.primaryAddress.country"
          />
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <satify-button
        type="button"
        @click.native="save"
        aria-label="save new customer"
      >
        Save Customer
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
import { Component, Vue } from "vue-property-decorator";
import { ICustomer } from "@/types/Customer";

import SatifyButton from "@/components/SatifyButton.vue";
import SatifyModal from "@/components/modals/SatifyModal.vue";

@Component({
  name: "NewCustomerModal",
  components: { SatifyButton, SatifyModal },
})
export default class NewCustomerModal extends Vue {
  customer: ICustomer = {
    createdOn: new Date(),
    updatedOn: new Date(),
    firstName: "",
    lastName: "",
    primaryAddress: {},
  };

  save() {
    this.$emit("save:customer", this.customer);
  }

  close() {
    this.$emit("close");
  }
}
</script>

<style scoped lang="scss">
.newCustomer {
  display: flex;
  flex-wrap: wrap;
  list-style: none;
  padding: 0;
  margin: 0;

  input {
    width: 80%;
    height: 1.8rem;
    margin: 0.8rem 2rem 0.8rem 0.8rem;
    font-size: 1.1rem;
    line-height: 1.3rem;
    padding: 0.2rem;
    color: #555;
  }

  label {
    font-weight: bold;
    margin: 0.8rem;
    display: block;
  }
}
</style>
